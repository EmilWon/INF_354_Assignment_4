import { MainService } from './../services/main.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ChartOptions } from 'chart.js';
import jsPDF from 'jspdf';
import { Label } from 'ng2-charts';
import html2canvas from 'html2canvas'

export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H'},
  {position: 2, name: 'Helium', weight: 4.0026, symbol: 'He'},
  {position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li'},
  {position: 4, name: 'Beryllium', weight: 9.0122, symbol: 'Be'},
  {position: 5, name: 'Boron', weight: 10.811, symbol: 'B'},
  {position: 6, name: 'Carbon', weight: 12.0107, symbol: 'C'},
  {position: 7, name: 'Nitrogen', weight: 14.0067, symbol: 'N'},
  {position: 8, name: 'Oxygen', weight: 15.9994, symbol: 'O'},
  {position: 9, name: 'Fluorine', weight: 18.9984, symbol: 'F'},
  {position: 10, name: 'Neon', weight: 20.1797, symbol: 'Ne'},
];

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit{

  constructor(/*public categories: MainService, public products: MainService*/) { }

  ngOnInit(): void{
    // this.categories.loadCategories().subscribe();
  }

  public genReport(): void {
    //var Dates
    // this.products.loadCategories().subscribe();
  }

  displayedColumns: string[] = ['position', 'name', 'weight', 'symbol'];
  dataSource = ELEMENT_DATA;

  barChartOptions: ChartOptions = {
    responsive: true,
  };
  barChartLabels: Label[] = ['One', 'Two', 'Three', 'Four', 'Five', 'Six'];
  barChartType: Chart.ChartType = 'bar';
  barChartLegend = true;
  barChartPlugins = [];

  barChartData: Chart.ChartDataSets[] = [
    { data: [30, 37, 60, 40, 40, 40], label: 'Average Products Ordered' }
  ];

  @ViewChild('htmlData') htmlData:ElementRef | any;

  public openPDF():void {
    let Data = document.getElementById('htmlData')!;
  
    html2canvas(Data).then(canvas => {
        let fileWidth = 210;
        let fileHeight = canvas.height * fileWidth / canvas.width;
  
        const contentDataURL = canvas.toDataURL('image/png')
  
  
        let PDF = new jsPDF({ orientation: 'p', unit: 'mm', format: 'a4',});
        let topPosition = 10;
        let leftPosition = 0;
        PDF.addImage(contentDataURL, 'PNG', leftPosition, topPosition, fileWidth, fileHeight)
        PDF.save('Graph.pdf');
    });
  }



}
