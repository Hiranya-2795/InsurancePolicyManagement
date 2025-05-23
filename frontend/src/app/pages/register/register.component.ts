import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerData = {
    FullName: '',
    DateOfBirth: '',
    Gender: '',
    PhoneNumber: '',
    Email: '',
    AadharNumber: '',
    Password: '',
    Role: 'User'
  };

  message: string | null = null;
  messageType: 'success' | 'error' | null = null;

  constructor(private http: HttpClient, private router: Router) {}

  showMessage(msg: string, type: 'success' | 'error' = 'error') {
    this.message = msg;
    this.messageType = type;
    setTimeout(() => {
      this.message = null;
      this.messageType = null;
    }, 4000);
  }

  register() {
    console.log('Sending registration data:', this.registerData);

    const apiUrl = 'https://localhost:7268/api/Auth/register';

    this.http.post(apiUrl, this.registerData, { responseType: 'text' }).subscribe({
      next: (response) => {
        this.showMessage('Registered successfully', 'success');
        // Navigate after showing success message for 2 seconds
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 3000);
      },
      error: (err) => {
        console.error('Registration failed:', err);
        if (err.status === 400) {
          this.showMessage('Invalid input or user already exists.', 'error');
        } else if (err.status === 0) {
          this.showMessage('Cannot reach server. Is your API running and CORS enabled?', 'error');
        } else {
          this.showMessage('Registration failed. Try again later.', 'error');
        }
      }
    });
  }
}
