import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { DxButtonModule, DxFormModule, DxPopupModule } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { Task, TasksService, ToDo } from '../../services/tasks.service';
import { TodoCardComponent, TodoCardModule } from '../todo-card/todo-card.component';

@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.scss']
})
export class TaskCardComponent implements OnInit {
  @ViewChild('todoCards', { read: ViewContainerRef }) todoCards!: ViewContainerRef;
  task: Task | null;
  workspaceId!: string;
  createToDoPopupIsVisible: boolean;
  todo: ToDo;

  constructor(private taskService: TasksService) {
    this.task = new Task();
    this.todo = new ToDo();
    this.createToDoPopupIsVisible = false;
  }

  async ngOnInit(): Promise<void> {
    try {
      var todos = await this.taskService.getToDos(this.task!, this.workspaceId);
      todos.forEach(todo => {
        var component = this.todoCards.createComponent(TodoCardComponent);
        component.instance.todo = todo;
      })
    }
    catch (e){
      console.log(e);
      //notify('Error when loading todos. Check the console for details', 'error');
    }
  }

  toggleCreateToDoPopup() {
    console.log('toggleCreateToDoPopup')
    this.createToDoPopupIsVisible = true;
  }

  async createToDo(e: any) {
    e.preventDefault();
    
    try {
      var todo = await this.taskService.createToDos(this.task!, this.workspaceId, this.todo);
      var component = this.todoCards.createComponent(TodoCardComponent);
      component.instance.todo = todo;
    }
    catch (e){
      console.log(e);
      notify('Error when creating todo. Check the console for details', 'error');
    }
  }

  async deleteTask() {
    console.log("sadfjksa");
  }
}

@NgModule({
  imports: [
    DxButtonModule,
    CommonModule,
    DxPopupModule,
    DxFormModule,
    TodoCardModule
  ],
  declarations: [ TaskCardComponent ],
  exports: [ TaskCardComponent ]
})
export class TaskCardModule { }
