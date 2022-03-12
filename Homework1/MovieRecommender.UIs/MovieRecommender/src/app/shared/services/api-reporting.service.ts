import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

export class Report {
  constructor(
    public apiRequests: APIRequest[],
    public numberOfRequestsOnYoutube: number, 
    public numberOfRequestsOnTMDb: number, 
    public numberOfRequestsOnSendGrid: number,
    public meanLatencyForYoutube: number,
    public meanLatencyForTMDb: number,
    public meanLatencyForSendGrid: number,
    public numberOfNotOkResponsesOnYoutube: number,
    public numberOfNotOkResponsesOnTMDb: number,
    public numberOfNotOkResponsesOnSendGrid: number,
    public latenciesForYoutube: number[],
    public latenciesForTMDb: number[],
    public latenciesForSendGrid: number[],) { 
  }
}

export class NumberOfRequestsReport {
  api!: string;
  numberOfRequests!: number;
}

export class APIPerformanceReporting {
  api!: string;
  numberOfRequests!: number;
  meanLatency!: number;
  numberOfNotOkResponses!: number;
}

export class APIRequest {
    constructor(
      public id: string,
      public provider: string, 
      public httpMethod: Date, 
      public endpoint: number,
      public requestContent: string,
      public responseContent: number,
      public latency: number) { 
    }
  }

@Injectable()
export class APIReportingService {
  constructor(private http: HttpClient) { }

  async getReport(): Promise<Report> {
    const response = await this.http
        .get<Report>('/api/1/Metrics')
        .toPromise();

    console.log(response);
    return response;
  }
}