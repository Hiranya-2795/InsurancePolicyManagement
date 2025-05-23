// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements CanActivate {

  constructor(private router: Router) {}

  getRole(): 'ADMIN' | 'CUSTOMER' {
    return localStorage.getItem('role') === 'ADMIN' ? 'ADMIN' : 'CUSTOMER';
  }

  isAdmin(): boolean {
    return this.getRole() === 'ADMIN';
  }

  isCustomer(): boolean {
    return this.getRole() === 'CUSTOMER';
  }

  // Implementing the CanActivate interface to protect routes
  canActivate(route: any): boolean {
    const role = this.getRole();
    const requiredRole = route.data?.role;

    if (requiredRole && requiredRole !== role) {
      this.router.navigate(['/']); // Redirect to login if not authorized
      return false;
    }

    return true; // Allow access
  }
}
