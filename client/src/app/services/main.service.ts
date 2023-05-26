import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from "rxjs/operators";

@Injectable()
export class MainService {

  constructor(private http: HttpClient) { }

  public categories = [];

  public products = [];

  // loadCategories() {
  //   return this.http.get<[]>("api/GetCategories")
  //     .pipe(map(data => {
  //       this.categories = data;
  //       return;
  //     }));
  // }

  //loadReport() {
  //  return this.http.get<[]>("api/GetReportData")
  //    .pipe(map(data => {
  //      this.products = data;
  //      return;
  //    }))
  //}

  //login() {
  //  retunr this.http.post<[]>("api/Login")
  //}
}
