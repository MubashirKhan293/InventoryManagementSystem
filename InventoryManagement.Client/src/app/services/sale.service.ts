import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateSaleDto, SaleDto } from '../models/sale.model';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class SaleService {
  
  private readonly APIUrl = environment.baseUrl + '/api/Sales';

  constructor(private http: HttpClient) { }

  // GET all sales
  getAllSales(): Observable<SaleDto[]> {
    return this.http.get<SaleDto[]>(`${this.APIUrl}/GetAllSales`);
  }

  // GET sale by ID
  getSaleById(id: number): Observable<SaleDto> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.get<SaleDto>(`${this.APIUrl}/GetSaleById`, { params });
  }

  // POST create new sale
  createSale(sale: CreateSaleDto): Observable<SaleDto> {
    return this.http.post<SaleDto>(`${this.APIUrl}/CreateSale`, sale, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  // GET sales by product ID
  getSalesByProduct(productId: number): Observable<SaleDto[]> {
    const params = new HttpParams().set('productId', productId.toString());
    return this.http.get<SaleDto[]>(`${this.APIUrl}/GetSalesByProduct`, { params });
  }
}
