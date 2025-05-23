
// src/app/services/policy.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Policy {
  policyID: string;
  policyType: string;
  startDate: string;
  endDate: string;
  policyTerm: number;
  coverageAmount: number;
  premiumAmount: number;
  premiumFrequency: string;
}

@Injectable({
  providedIn: 'root',
})
export class PolicyService {
  private apiUrl = 'https://localhost:7268/api/Policy'; // <-- Use your actual backend URL

  constructor(private http: HttpClient) {}

  // Get all policies
  getPolicies(): Observable<Policy[]> {
    return this.http.get<Policy[]>(this.apiUrl);
  }

  // Get a single policy by ID
  getPolicyById(id: string): Observable<Policy> {
    return this.http.get<Policy>(`${this.apiUrl}/${id}`);
  }

  // Create a new policy
  addPolicy(policy: Policy): Observable<Policy> {
    return this.http.post<Policy>(this.apiUrl, policy);
  }

  // Update an existing policy
  updatePolicy(id: string, policy: Policy): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, policy);
  }

  // Delete a policy
  deletePolicy(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Search policies
  searchPolicies(
    policyId?: string,
    policyType?: string,
    premiumFrequency?: string
  ): Observable<Policy[]> {
    let query = `${this.apiUrl}/search?`;
    if (policyId) query += `policyId=${encodeURIComponent(policyId)}&`;
    if (policyType) query += `policyType=${encodeURIComponent(policyType)}&`;
    if (premiumFrequency) query += `premiumFrequency=${encodeURIComponent(premiumFrequency)}&`;
    return this.http.get<Policy[]>(query.slice(0, -1)); // remove trailing &
  }
}
