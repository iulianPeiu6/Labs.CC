import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { lastValueFrom } from 'rxjs';
import { environment as env } from 'src/environments/environment';

export class Task {
  id!: string;
  title!: string;
  description!: string;
  dueDateTime!: Date;
  toDoChecks!: Array<ToDo>;
  boardId!: string;
}

export class ToDo {
  id!: string;
  description!: string;
  completed!: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class TasksService {

  constructor(private http: HttpClient, private auth: AuthService) { }

  async getTasksFromBord(workspaceId: string, boardId: string) {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.get<Array<Task>>(`${env.apiEndpoint}/api/v1/Workspaces/${workspaceId}/Boards/${boardId}/Tasks`, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }

  async create(task: Task, workspaceId: String): Promise<Task> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const boardToCreate = {
      title: task.title,
      description: task.description,
      dueDateTime: task.dueDateTime,
      boardId: task.boardId
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.post<Task>(`${env.apiEndpoint}/api/v1/Workspaces/${workspaceId}/Boards/${task.boardId}/Tasks`, boardToCreate, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }

  async getToDos(task: Task, workspaceId: String): Promise<Array<ToDo>> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.get<Array<ToDo>>(`${env.apiEndpoint}/api/v1/Workspaces/${workspaceId}/Boards/${task.boardId}/Tasks/${task.id}/ToDos`, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }

  async createToDos(task: Task, workspaceId: String, todo: ToDo): Promise<ToDo> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const todoToCreate = {
      description: todo.description
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.post<ToDo>(`${env.apiEndpoint}/api/v1/Workspaces/${workspaceId}/Boards/${task.boardId}/Tasks/${task.id}/ToDos`, todoToCreate,  { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }
}
