import { Injectable } from '@angular/core';
import { environment } from '../environment/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ProductDto, ProductInputDto } from '../models/product.model';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  
  readonly APIUrl = environment.baseUrl + '/api/Products';

  constructor(private http: HttpClient) { }

  getAllProducts(): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.APIUrl}/GetAllProducts`);
  }

  getProductById(id: number): Observable<ProductDto> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.get<ProductDto>(`${this.APIUrl}/GetProductById`, { params });
  }

  createProduct(product: ProductInputDto): Observable<ProductDto> {
    return this.http.post<ProductDto>(`${this.APIUrl}/CreateProduct`, product);
  }

  updateProduct(id: number, product: ProductInputDto): Observable<ProductDto> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.put<ProductDto>(`${this.APIUrl}/UpdateProduct`, product, { params });
  }

  deleteProduct(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.delete<any>(`${this.APIUrl}/DeleteProduct`, { params });
  }
}
