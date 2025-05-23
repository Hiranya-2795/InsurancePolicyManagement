// src/app/pages/view-policy/view-policy.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { PolicyService, Policy } from '../../services/policy.service';

@Component({
  standalone: true,
  selector: 'app-view-policy',
  templateUrl: './view-policy.component.html',
  styleUrls: ['./view-policy.component.scss'],
  imports: [CommonModule, RouterModule],
})
export class ViewPolicyComponent implements OnInit {
  policy: Policy | null = null;
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private policyService: PolicyService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.policyService.getPolicyById(id).subscribe({
        next: (data: Policy) => {
          this.policy = data;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error fetching policy:', error);
          this.loading = false;
        }
      });
    } else {
      console.warn('No policy ID found in route.');
      this.loading = false;
    }
  }
}
