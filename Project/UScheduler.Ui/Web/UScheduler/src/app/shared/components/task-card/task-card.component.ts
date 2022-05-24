import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { DxButtonModule, DxFormModule, DxPopupModule } from 'devextreme-angular';
import { Task } from '../../services/tasks.service';

@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.scss']
})
export class TaskCardComponent implements OnInit {
  task: Task | null;
  constructor() {
    this.task = new Task();
  }

  ngOnInit(): void {
  }

}

@NgModule({
  imports: [
    DxButtonModule,
    CommonModule,
    DxPopupModule,
    DxFormModule
  ],
  declarations: [ TaskCardComponent ],
  exports: [ TaskCardComponent ]
})
export class TaskCardModule { }
