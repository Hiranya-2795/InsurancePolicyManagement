import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { PolicyService, Policy } from '../../services/policy.service';

@Component({
  selector: 'app-edit-policy',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './edit-policy.component.html',
  styleUrls: ['./edit-policy.component.scss']
})
export class EditPolicyComponent implements OnInit {
  policy: Policy = {
    policyID: '',
    policyType: '',
    startDate: '',
    endDate: '',
    policyTerm: 0,
    coverageAmount: 0,
    premiumAmount: 0,
    premiumFrequency: ''
  };

  message: string | null = null;
  messageType: 'success' | 'error' | null = null;

  constructor(
    private policyService: PolicyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const policyId = this.route.snapshot.paramMap.get('id');
    if (policyId) {
      this.policyService.getPolicyById(policyId).subscribe({
        next: (data) => {
          this.policy = {
            ...data,
            startDate: data.startDate ? data.startDate.slice(0, 10) : '',
            endDate: data.endDate ? data.endDate.slice(0, 10) : '',
            policyType: this.capitalizeFirstLetter(data.policyType),
            premiumFrequency: this.capitalizeFirstLetter(data.premiumFrequency)
          };
        },
        error: (err) => {
          this.showMessage('Error loading policy data.', 'error');
          console.error(err);
        }
      });
    }
  }

  capitalizeFirstLetter(text: string): string {
    if (!text) return '';
    return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
  }

  showMessage(msg: string, type: 'success' | 'error' = 'error') {
    this.message = msg;
    this.messageType = type;
    setTimeout(() => {
      this.message = null;
      this.messageType = null;
    }, 4000);
  }

  onSubmit(): void {
    const errors: string[] = [];

    if (!this.policy.policyID) errors.push('Policy ID is required.');
    if (!this.policy.policyType) errors.push('Policy Type is required.');
    if (!this.policy.startDate) errors.push('Start Date is required.');
    if (!this.policy.endDate) errors.push('End Date is required.');
    if (!this.policy.policyTerm || this.policy.policyTerm <= 0) errors.push('Policy Term must be greater than 0.');
    if (!this.policy.coverageAmount || this.policy.coverageAmount <= 0) errors.push('Coverage Amount must be greater than 0.');
    if (!this.policy.premiumAmount || this.policy.premiumAmount <= 0) errors.push('Premium Amount must be greater than 0.');
    if (!this.policy.premiumFrequency) errors.push('Payment Frequency is required.');

    if (errors.length > 0) {
      this.showMessage('Please fill in all required fields:\n\n' + errors.join('\n'), 'error');
      return;
    }

    this.policyService.updatePolicy(this.policy.policyID, this.policy).subscribe({
      next: () => {
        this.showMessage('Policy successfully updated!', 'success');
        // Wait 2 seconds to show success message then navigate
        setTimeout(() => {
          this.message = null;
          this.messageType = null;
          this.router.navigate(['/admin-dashboard']);
        }, 2000);
      },
      error: (err) => {
        this.showMessage('Error updating policy.', 'error');
        console.error(err);
      }
    });
  }
}
