import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, FormsModule],
  templateUrl: './customer-dashboard.component.html',
  styleUrls: ['./customer-dashboard.component.scss']
})
export class CustomerDashboardComponent implements OnInit {
  searchQuery: string = '';
  policies: any[] = [];
  allPolicies: any[] = [];

  constructor(public userService: UserService, private router: Router) {}

  ngOnInit(): void {
    const userId = this.userService.getUserId();
    if (userId) {
      this.userService.getPoliciesByUserId(userId).subscribe({
        next: (data) => {
          this.policies = data;
          this.allPolicies = data;
        },
        error: (err) => {
          console.error('Failed to load policies:', err);
        }
      });
    }
  }

  searchPolicies(): void {
    const query = this.searchQuery.trim().toLowerCase();

    if (!query) {
      this.policies = [...this.allPolicies];
      return;
    }

    this.policies = this.allPolicies.filter(policy =>
      (policy.policyID && policy.policyID.toLowerCase().includes(query)) ||
      (policy.policy?.policyType && policy.policy.policyType.toLowerCase().includes(query)) ||
      (policy.policy?.premiumFrequency && policy.policy.premiumFrequency.toLowerCase().includes(query)) ||
      (policy.beneficiaryName && policy.beneficiaryName.toLowerCase().includes(query))
    );
  }

  logout(): void {
    this.userService.logout();
    this.router.navigate(['/']);
  }
}
