import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';

@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(private router: Router, private auth: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    this.auth.isAuthenticated$.subscribe(loggedIn =>{
      if (loggedIn === false) {
        this.auth.loginWithRedirect();
      }
      return true;
    });

    return true;
  }
}
