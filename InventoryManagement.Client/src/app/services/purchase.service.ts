import { Injectable } from '@angular/core';
import { environment } from '../environment/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CreatePurchaseDto, PurchaseDto } from '../models/purchase.model';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {

  private readonly APIUrl = environment.baseUrl + '/api/Purchases';

  constructor(private http: HttpClient) { }

  // GET all purchases
  getAllPurchases(): Observable<PurchaseDto[]> {
    return this.http.get<PurchaseDto[]>(`${this.APIUrl}/GetAllPurchases`);
  }

  // GET purchase by ID
  getPurchaseById(id: number): Observable<PurchaseDto> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.get<PurchaseDto>(`${this.APIUrl}/GetPurchaseById`, { params });
  }

  // POST create new purchase
  createPurchase(purchase: CreatePurchaseDto): Observable<PurchaseDto> {
    return this.http.post<PurchaseDto>(`${this.APIUrl}/CreatePurchase`, purchase);
  }

  // GET purchases by product ID
  getPurchasesByProduct(productId: number): Observable<PurchaseDto[]> {
    const params = new HttpParams().set('productId', productId.toString());
    return this.http.get<PurchaseDto[]>(`${this.APIUrl}/GetPurchasesByProduct`, { params });
  }

  // PUT update purchase status (commented out as per your controller)
  /*
  updatePurchaseStatus(id: number, statusDto: UpdatePurchaseStatusDto): Observable<PurchaseDto> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.put<PurchaseDto>(`${this.APIUrl}/UpdatePurchaseStatus`, statusDto, { params });
  }
  */
}