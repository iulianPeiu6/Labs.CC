import { HttpClient } from '@angular/common/http';
import { Component, HostBinding } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { ScreenService, AppInfoService } from './shared/services';
import { WorkspacesService } from './shared/services/workspaces.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent  {
  @HostBinding('class') get getClass() {
    return Object.keys(this.screen.sizes).filter(cl => this.screen.sizes[cl]).join(' ');
  }

  constructor(
    public auth: AuthService, 
    private screen: ScreenService, 
    public appInfo: AppInfoService) {

  }
}