import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Product } from '../shared/models/Product';
 import 'rxjs/Rx' ;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public productBlocks: Product[];
  apiUrl: string;
  token:string="";
  webRootPath:string;
 
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    this.apiUrl = baseUrl + 'api/Product/';
    this.getAllProducts();
  }
  getAllProducts() {
    this.http.get<Product[]>(this.apiUrl).subscribe(result => {
      this.productBlocks = result;
    }, error => console.error(error));
  }
  searchProduct() {
    if(this.token && this.token.length>0)
      this.http.get<Product[]>(this.apiUrl + 'Search/'+ this.token).subscribe(result => {
        this.productBlocks = result;
      }, error => console.error(error));
    else
      this.getAllProducts();
  }
  editProduct(productId) {
    this.router.navigate(['product/', productId]);
  }
  deleteProduct(productId) {
    this.http.delete(this.apiUrl + productId)
      .subscribe(
          success=>{
            alert("Product Deleted Successfully");
            this.getAllProducts();
          },
          error => console.error(error)
          );
  }
  downloadExcel() { //get file from service
        this.http.get(this.apiUrl + 'ExportProduct/'+ this.token,{responseType: 'blob'})
       .subscribe(data => this.downloadFile(data)),//console.log(data),
                 error => console.log("Error downloading the file."),
                 () => console.info("OK");
  }
  downloadFile(data){
    var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    var url= window.URL.createObjectURL(blob);
    window.open(url);
  }
}

