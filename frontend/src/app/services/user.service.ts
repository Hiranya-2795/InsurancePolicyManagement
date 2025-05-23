// src/app/services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userProfileUrl = 'https://localhost:7268/api/UserProfile';
  private policyUrl = 'https://localhost:7268/api/UserPolicy';

  constructor(private http: HttpClient) {}

  // LocalStorage helpers
  getUserId(): string | null {
    return localStorage.getItem('userId');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

  logout(): void {
    localStorage.clear();
  }

  // User profile methods
  getUserById(userId: number): Observable<any> {
    return this.http.get(`${this.userProfileUrl}/${userId}`);
  }

  updateUser(userId: number, data: any) {
    return this.http.put(`${this.userProfileUrl}/${userId}`, data);
  }


  // Policy methods
  getPoliciesByUserId(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.policyUrl}/user/${userId}`);
  }
}
