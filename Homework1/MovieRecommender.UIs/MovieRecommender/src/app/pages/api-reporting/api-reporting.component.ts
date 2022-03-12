import { CommonModule } from '@angular/common';
import { Component, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { DxButtonModule, DxChartModule, DxCheckBoxModule, DxDataGridModule, DxLoadIndicatorModule, DxLoadPanelModule, DxLookupModule, DxPieChartModule, DxResponsiveBoxModule, DxSelectBoxModule, DxTemplateModule, DxTextAreaModule } from 'devextreme-angular';
import { DxiColModule, DxiColumnModule, DxiItemComponent } from 'devextreme-angular/ui/nested';
import ArrayStore from 'devextreme/data/array_store';
import 'devextreme/data/odata/store';
import { APIPerformanceReporting, APIReportingService, APIRequest, NumberOfRequestsReport, Report } from 'src/app/shared/services/api-reporting.service';

@Component({
  templateUrl: 'api-reporting.component.html'
})

export class APIReportingComponent {
  dataSource: any;
  report: Report | undefined;
  loadingVisible = false;
  numberOfRequestsReport: NumberOfRequestsReport[];
  apiPerformanceReport: APIPerformanceReporting[];
  providers = ['Youtube','TMDb','SendGrid'];
  httpMethods = ['GET', 'POST'];
  
  constructor(private apiReportingService: APIReportingService) {
    this.loadingVisible = true;
    this.numberOfRequestsReport = new Array<NumberOfRequestsReport>();
    this.apiPerformanceReport = new Array<APIPerformanceReporting>();
    this.dataSource =  new ArrayStore({
      key: ["id"],
      data: new Array<APIRequest>()
    });
  }

  async ngOnInit() {
    this.loadingVisible = true;
    this.report = await this.apiReportingService.getReport();

    this.numberOfRequestsReport = [{
      api: 'Youtube',
      numberOfRequests: this.report.numberOfRequestsOnYoutube,
    }, {
      api: 'TMDb',
      numberOfRequests: this.report.numberOfRequestsOnTMDb,
    }, {
      api: 'SendGrid',
      numberOfRequests: this.report.numberOfRequestsOnSendGrid,
    }];

    this.apiPerformanceReport = [{
      api: 'Youtube',
      numberOfRequests: this.report.numberOfRequestsOnYoutube,
      meanLatency: this.report.meanLatencyForYoutube / 1000,
      numberOfNotOkResponses: this.report.numberOfNotOkResponsesOnYoutube
    }, {
      api: 'TMDb',
      numberOfRequests: this.report.numberOfRequestsOnTMDb,
      meanLatency: this.report.meanLatencyForTMDb / 1000,
      numberOfNotOkResponses: this.report.numberOfNotOkResponsesOnTMDb
    }, {
      api: 'SendGrid',
      numberOfRequests: this.report.numberOfRequestsOnSendGrid,
      meanLatency: this.report.meanLatencyForSendGrid / 1000,
      numberOfNotOkResponses: this.report.numberOfNotOkResponsesOnSendGrid
    }];

    this.dataSource =  new ArrayStore({
      key: ["id"],
      data: this.report.apiRequests
    });
    this.loadingVisible = false;
  }

  pointClickHandler(e: Event) {
    this.toggleVisibility(e.target);
  }

  legendClickHandler(e: any) {
    const arg = e.target;
    const item = e.component.getAllSeries()[0].getPointsByArg(arg)[0];

    this.toggleVisibility(item);
  }

  toggleVisibility(item: any) {
    if (item.isVisible()) {
      item.hide();
    } else {
      item.show();
    }
  }
}

@NgModule({
  imports: [
    CommonModule,
    DxLoadIndicatorModule,
    DxLoadPanelModule, 
    DxDataGridModule,
    RouterModule,
    DxButtonModule,
    DxLoadIndicatorModule,
    BrowserModule,
    DxTextAreaModule,
    DxCheckBoxModule,
    DxSelectBoxModule,
    DxPieChartModule,
    DxResponsiveBoxModule,
    DxTemplateModule,
    DxChartModule,
    DxLookupModule,
    DxiColumnModule],
  exports: [APIReportingComponent],
  declarations: [APIReportingComponent],
  providers: [],
})
export class APIReportingModule { }
