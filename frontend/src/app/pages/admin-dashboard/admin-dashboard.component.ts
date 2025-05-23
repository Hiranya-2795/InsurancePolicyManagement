import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PolicyService,Policy } from '../../services/policy.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  searchQuery: string = '';
  policies: Policy[] = [];
  filteredPolicies: Policy[] = [];
  loading: boolean = false;
  error: string = '';

  constructor(private policyService: PolicyService) {}

  ngOnInit(): void {
    this.fetchPolicies();
  }

  fetchPolicies(): void {
    this.loading = true;
    this.policyService.getPolicies().subscribe({
      next: (data) => {
        this.policies = data;
        this.filteredPolicies = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching policies:', err);
        this.error = 'Failed to load policies.';
        this.loading = false;
      }
    });
  }

  searchPolicies(): void {
    const query = this.searchQuery.toLowerCase().trim();
    this.filteredPolicies = this.policies.filter(policy =>
      policy.policyID.toLowerCase().includes(query) ||
      policy.policyType.toLowerCase().includes(query) ||
      policy.premiumFrequency.toLowerCase().includes(query)
    );
  }
}