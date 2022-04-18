import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { environment as env } from 'src/environments/environment';
import { lastValueFrom } from 'rxjs';

export class Workspace {
    id!: string;
    title!: string;
    description!: string;
    owner!: string;
    accessType!: string;
    colabs!: Array<string>;
    workspaceType!: string;
    createdAt!: Date;
    createdBy!: string;
    updatedAt!: Date;
    updatedBy!: string;
}

@Injectable()
export class WorkspacesService {
  constructor(private http: HttpClient, private auth: AuthService) {}

  async createWorkspace(workspace: Workspace) {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const workspaceToCreate = {
      title: workspace.title,
      description: workspace.description,
      accessType: workspace.accessType,
      workspaceType: workspace.workspaceType
    }

    //ToDo: Fix
    let createdBy = (await lastValueFrom(this.auth.getUser()))?.email
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'CreatedBy': createdBy!,
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.post<Workspace>('/api/v1/Workspaces', workspaceToCreate, { headers: headers});
    let response = await lastValueFrom(response$);

    console.log(response);
  }
}