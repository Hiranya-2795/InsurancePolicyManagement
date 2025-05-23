import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { ActivatedRoute, Router } from '@angular/router'; // Import Router
import { timer } from 'rxjs'; // RxJS timer for delay

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {
  profile = {
    fullName: '',
    dob: '',
    gender: '',
    phone: '',
    email: '',
    aadharNo: '',
    password: '',
    role: 'User' // Default fallback role
  };

  errors: string[] = [];
  message: string = '';
  messageType: 'success' | 'error' = 'success';
  userId: number = 0;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router // Inject Router
  ) {}

  ngOnInit(): void {
    const storedId = localStorage.getItem('userId');
    if (storedId) {
      this.userId = +storedId;
      this.userService.getUserById(this.userId).subscribe({
        next: (data) => {
          this.profile = {
            fullName: data.fullName || '',
            dob: data.dateOfBirth ? data.dateOfBirth.split('T')[0] : '',
            gender: data.gender || '',
            phone: data.phoneNumber || '',
            email: data.email || '',
            aadharNo: data.aadharNumber || '',
            password: '', // do not pre-fill password
            role: data.role || 'User'
          };
        },
        error: () => {
          this.showMessage('Failed to load profile data.', 'error');
        }
      });
    } else {
      this.showMessage('User ID not found.', 'error');
    }
  }

  onSubmit(form: NgForm): void {
    this.errors = [];
    this.clearMessage();

    if (!this.profile.fullName || !/^[A-Za-z\s]+$/.test(this.profile.fullName)) {
      this.errors.push('Full Name is required and must contain only letters.');
    }
    if (!this.profile.dob) {
      this.errors.push('Date of Birth is required.');
    }
    if (!this.profile.gender) {
      this.errors.push('Gender is required.');
    }
    if (!this.profile.phone || !/^\d{10}$/.test(this.profile.phone)) {
      this.errors.push('Phone Number must be exactly 10 digits.');
    }
    if (!this.profile.email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(this.profile.email)) {
      this.errors.push('A valid Email Address is required.');
    }
    if (!this.profile.aadharNo || !/^\d{12}$/.test(this.profile.aadharNo)) {
      this.errors.push('Aadhar Number must be exactly 12 digits.');
    }
    if (this.profile.password && !/^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,20}$/.test(this.profile.password)) {
      this.errors.push('Password must be 8–20 chars, include 1 uppercase, 1 number, 1 special char.');
    }

    if (this.errors.length) {
      this.showMessage('Please fix the errors before submitting.', 'error');
      return;
    }

    const updatePayload: any = {
      UserID: this.userId,
      FullName: this.profile.fullName,
      DateOfBirth: this.profile.dob,
      Gender: this.profile.gender,
      PhoneNumber: this.profile.phone,
      Email: this.profile.email,
      AadharNumber: this.profile.aadharNo,
      Password: this.profile.password ? this.profile.password : null,
      Role: this.profile.role // Include Role for server validation
    };

    this.userService.updateUser(this.userId, updatePayload).subscribe({
      next: () => {
        this.showMessage('Profile updated successfully.', 'success');
        this.profile.password = ''; // clear password after success
        
        // Wait 3 seconds then redirect to customer dashboard
        timer(3000).subscribe(() => {
          this.router.navigate(['/customer-dashboard']);
        });
      },
      error: () => {
        this.showMessage('Failed to update profile. Reenter your password.', 'error');
      }
    });
  }

  private showMessage(msg: string, type: 'success' | 'error') {
    this.message = msg;
    this.messageType = type;
  }

  private clearMessage() {
    this.message = '';
  }
}
