import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { lastValueFrom } from 'rxjs';
import { environment as env } from 'src/environments/environment';

export class Board {
  id!: string;
  title!: string;
  description!: string;
  workspaceId!: string;
}

@Injectable({
  providedIn: 'root'
})
export class BoardsService {

  constructor(private http: HttpClient, private auth: AuthService) { 

  }

  async getBoardsFromWorksopace(id: String): Promise<Array<Board>> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.get<Array<Board>>(`${env.apiEndpoint}/api/v1/Workspaces/${id}/Boards`, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }

  async create(board: Board, workspaceId: String): Promise<Board> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const boardToCreate = {
      title: board.title,
      description: board.description,
      workspaceId: workspaceId
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.post<Board>(`${env.apiEndpoint}/api/v1/Workspaces/${workspaceId}/Boards`, boardToCreate, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }

  async delete(board: Board) {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.delete(`${env.apiEndpoint}/api/v1/Workspaces/${board.workspaceId}/Boards/${board.id}`, { headers: headers});
    await lastValueFrom(response$);
  }
}
