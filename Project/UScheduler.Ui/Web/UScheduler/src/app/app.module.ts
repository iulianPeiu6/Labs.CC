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
import { WorkspacesService } from './shared/services/workspaces.service';
import { HttpClientModule } from '@angular/common/http';
import { BoardsService } from './shared/services/boards.service';
import { TasksService } from './shared/services/tasks.service';

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
    AuthModule.forRoot({
      ... env.auth
    })
  ],
  providers: [
    ScreenService,
    AppInfoService,
    WorkspacesService,
    BoardsService,
    TasksService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
