import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders,HttpRequest,HttpEventType } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../shared/models/Product';

@Component({
  selector: 'app-home',
  templateUrl: './product.component.html',
})
export class ProductComponent {
  isEdit: boolean = false;
  productId: number;
  productItem: Product;
  apiUrl: string;
  headers: HttpHeaders;
  progress:string;
  uploadMessage:string;
  @ViewChild("photoInput") photoInput;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute,private router: Router) {
    this.productId = route.snapshot.params['productId'];
    this.productItem = new Product();
    this.apiUrl = baseUrl + 'api/Product/';
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    if (this.productId)
      this.getProductDetails();
    
  }
  getProductDetails() {
      this.isEdit = true;
      this.http.get<Product>(this.apiUrl + this.productId).subscribe(result => {
        this.productItem = result;
      }, error => console.error(error));
  }

  updateProduct() {
    let files=this.photoInput.nativeElement.files;
    if(this.productItem.name.length<2){
      alert("Product name should have more than 2 characters");
      return;
    }else if(!this.productItem.price){
      alert("Please enter a price");
      return;
    }

    if (this.isEdit)
      this.http.put(this.apiUrl, this.productItem, { headers: this.headers })
        .subscribe(
          success=>{
            this.uploadPhoto(files,this.productItem.id);
          },
          error => console.error(error)
          );
    else
      this.http.post(this.apiUrl, this.productItem, { headers: this.headers })
      .subscribe((result:any)=>{
          this.uploadPhoto(files,result);
        },
        error => console.error(error)
        );
  }

  uploadPhoto(files,productId){
      if(files && files.length>0){
        let file=files[0];
        let data=new FormData();
        var ext=file.name.substr(file.name.lastIndexOf('.'));
        data.append(file.name,file,productId+ext);
        this.http.request(new HttpRequest('POST',this.apiUrl+'UploadPhoto/'+productId,data,{reportProgress:true}))
        .subscribe(event=>{
          if(event.type===HttpEventType.UploadProgress)
            this.progress="Uploading File : "+Math.round(100 * event.loaded / event.total)+" %";
          else if(event.type===HttpEventType.Response){
            alert("Product Updated Successfully");
            this.router.navigate(['']);
          }
        })
      }
      else{
        alert("Product Updated Successfully");
        this.router.navigate(['']);
      }
  }
  cancelUpdate(){
    this.router.navigate(['']);
  }
}
