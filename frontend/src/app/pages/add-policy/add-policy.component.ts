import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { PolicyService, Policy } from '../../services/policy.service';

@Component({
  selector: 'app-add-policy',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './add-policy.component.html',
  styleUrls: ['./add-policy.component.scss']
})
export class AddPolicyComponent {
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

  message: string = '';
  messageType: 'success' | 'error' | '' = '';

  constructor(
    private policyService: PolicyService,
    private router: Router
  ) {}

  onSubmit(form: NgForm) {
    const errors: string[] = [];

    if (!this.policy.policyID) errors.push('Policy ID is required.');
    if (!this.policy.policyType) errors.push('Policy Type is required.');
    if (!this.policy.startDate) errors.push('Start Date is required.');
    if (!this.policy.endDate) errors.push('End Date is required.');
    if (!this.policy.policyTerm || this.policy.policyTerm <= 0) errors.push('Policy Term is required.');
    if (!this.policy.coverageAmount || this.policy.coverageAmount <= 0) errors.push('Coverage Amount is required.');
    if (!this.policy.premiumAmount || this.policy.premiumAmount <= 0) errors.push('Premium Amount is required.');
    if (!this.policy.premiumFrequency) errors.push('Payment Frequency is required.');

    if (errors.length) {
      this.setMessage('Please fix the following errors:\n' + errors.map(e => `• ${e}`).join('\n'), 'error');
      return;
    }

    this.policyService.addPolicy(this.policy).subscribe({
      next: () => {
        this.setMessage('Policy added successfully!', 'success');
        form.resetForm();
        setTimeout(() => this.router.navigate(['/admin-dashboard']), 2000);
      },
      error: (err) => {
        console.error('Error adding policy:', err);
        this.setMessage('Failed to add policy.', 'error');
      }
    });
  }

  setMessage(msg: string, type: 'success' | 'error') {
    this.message = msg;
    this.messageType = type;
    setTimeout(() => this.clearMessage(), 8000);
  }

  clearMessage() {
    this.message = '';
    this.messageType = '';
  }
}
