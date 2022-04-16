import { Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  templateUrl: 'profile.component.html',
  styleUrls: [ './profile.component.scss' ]
})

export class ProfileComponent {
  colCountByScreen: object;

  constructor(public auth: AuthService) {
    this.colCountByScreen = {
      xs: 1,
      sm: 2,
      md: 3,
      lg: 4
    };
  }
}
