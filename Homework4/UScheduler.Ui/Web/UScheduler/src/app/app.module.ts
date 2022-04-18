import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { SideNavOuterToolbarModule, SideNavInnerToolbarModule, SingleCardModule } from './layouts';
import { FooterModule } from './shared/components';
import { ScreenService, AppInfoService } from './shared/services';
import { UnauthenticatedContentModule } from './unauthenticated-content';
import { AppRoutingModule } from './app-routing.module';
import { AuthModule } from '@auth0/auth0-angular';
import { environment as env } from 'src/environments/environment';
import { CreateWorkspaceModule } from './shared/components/create-workspace/create-workspace.component';
import { WorkspacesService } from './shared/services/workspaces.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    SideNavOuterToolbarModule,
    SideNavInnerToolbarModule,
    SingleCardModule,
    FooterModule,
    UnauthenticatedContentModule,
    AppRoutingModule,
    CreateWorkspaceModule,
    AuthModule.forRoot({
      ... env.auth
    })
  ],
  providers: [
    ScreenService,
    AppInfoService,
    WorkspacesService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
