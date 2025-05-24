import { Component } from '@angular/core';
import { CreatePurchaseDto, PurchaseDto } from '../../models/purchase.model';
import { ProductDto } from '../../models/product.model';
import { PurchaseService } from '../../services/purchase.service';
import { ProductService } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

@Component({
  selector: 'app-purchases',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgxDatatableModule],
  templateUrl: './purchases.component.html',
  styleUrl: './purchases.component.scss'
})
export class PurchasesComponent {
 purchases: PurchaseDto[] = [];
  filteredPurchases: PurchaseDto[] = [];
  products: ProductDto[] = [];
  searchTerm: string = '';
  showAddForm: boolean = false;
  
  purchaseForm: CreatePurchaseDto = {
    productId: 0,
    quantityPurchased: 0,
    unitPrice: 0,
    supplierName: ''
  };

  // Pagination
  currentPage = 0;
  pageSize = 10;
  totalElements = 0;

  constructor(
    private purchaseService: PurchaseService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.loadPurchases();
    this.loadProducts();
  }

  loadPurchases(): void {
    this.purchaseService.getAllPurchases().subscribe({
      next: (data) => {
        this.purchases = data;
        this.filteredPurchases = data;
        this.totalElements = data.length;
      },
      error: (error) => console.error('Error loading purchases:', error)
    });
  }

  loadProducts(): void {
    this.productService.getAllProducts().subscribe({
      next: (data) => {
        this.products = data;
      },
      error: (error) => console.error('Error loading products:', error)
    });
  }

  onSearch(): void {
    if (!this.searchTerm) {
      this.filteredPurchases = this.purchases;
    } else {
      this.filteredPurchases = this.purchases.filter(purchase =>
        purchase.productName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        purchase.supplierName.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }
    this.totalElements = this.filteredPurchases.length;
    this.currentPage = 0;
  }

  onAddPurchase(): void {
    this.showAddForm = true;
    this.resetForm();
  }

  onSubmit(): void {
    this.purchaseService.createPurchase(this.purchaseForm).subscribe({
      next: () => {
        this.loadPurchases();
        this.loadProducts();
        this.cancelForm();
      },
      error: (error) => console.error('Error creating purchase:', error)
    });
  }

  cancelForm(): void {
    this.showAddForm = false;
    this.resetForm();
  }

  resetForm(): void {
    this.purchaseForm = {
      productId: 0,
      quantityPurchased: 0,
      unitPrice: 0,
      supplierName: ''
    };
  }

  onPageChange(event: any): void {
    this.currentPage = event.offset;
  }

  calculateTotal(): number {
    return this.purchaseForm.quantityPurchased * this.purchaseForm.unitPrice;
  }
}
