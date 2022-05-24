import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { DxButtonModule } from 'devextreme-angular';
import { ToDo } from '../../services/tasks.service';

@Component({
  selector: 'app-todo-card',
  templateUrl: './todo-card.component.html',
  styleUrls: ['./todo-card.component.scss']
})
export class TodoCardComponent implements OnInit {
  todo: ToDo;

  constructor() {
    this.todo = new ToDo();
  }

  ngOnInit(): void {
  }
}

@NgModule({
  imports: [
    DxButtonModule,
    CommonModule
  ],
  declarations: [ TodoCardComponent ],
  exports: [ TodoCardComponent ]
})
export class TodoCardModule { }
