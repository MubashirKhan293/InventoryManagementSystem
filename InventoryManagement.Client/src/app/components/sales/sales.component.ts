import { CommonModule } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ColumnMode, DatatableComponent, NgxDatatableModule, SelectionType, SortType } from '@swimlane/ngx-datatable';
import { CreateSaleDto, SaleDto } from '../../models/sale.model';
import { ProductDto } from '../../models/product.model';
import { SaleService } from '../../services/sale.service';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-sales',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgxDatatableModule],
  templateUrl: './sales.component.html',
  styleUrl: './sales.component.scss'
})
export class SalesComponent {
 sales: SaleDto[] = [];
  filteredSales: SaleDto[] = [];
  products: ProductDto[] = [];
  searchTerm: string = '';
  showAddForm: boolean = false;
  
  saleForm: CreateSaleDto = {
    productId: 0,
    quantitySold: 0,
    unitPrice: 0,
    customerName: ''
  };

  // Pagination
  currentPage = 0;
  pageSize = 10;
  totalElements = 0;

  constructor(
    private saleService: SaleService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.loadSales();
    this.loadProducts();
  }

  loadSales(): void {
    this.saleService.getAllSales().subscribe({
      next: (data) => {
        this.sales = data;
        this.filteredSales = data;
        this.totalElements = data.length;
      },
      error: (error) => console.error('Error loading sales:', error)
    });
  }

  loadProducts(): void {
    this.productService.getAllProducts().subscribe({
      next: (data) => {
        this.products = data.filter(p => p.quantity > 0);
      },
      error: (error) => console.error('Error loading products:', error)
    });
  }

  onSearch(): void {
    if (!this.searchTerm) {
      this.filteredSales = this.sales;
    } else {
      this.filteredSales = this.sales.filter(sale =>
        sale.productName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        sale.customerName.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }
    this.totalElements = this.filteredSales.length;
    this.currentPage = 0;
  }

  onAddSale(): void {
    this.showAddForm = true;
    this.resetForm();
  }

  onSubmit(): void {
    this.saleService.createSale(this.saleForm).subscribe({
      next: () => {
        this.loadSales();
        this.loadProducts();
        this.cancelForm();
      },
      error: (error) => console.error('Error creating sale:', error)
    });
  }

  cancelForm(): void {
    this.showAddForm = false;
    this.resetForm();
  }

  resetForm(): void {
    this.saleForm = {
      productId: 0,
      quantitySold: 0,
      unitPrice: 0,
      customerName: ''
    };
  }

  onPageChange(event: any): void {
    this.currentPage = event.offset;
  }

  getSelectedProduct(): ProductDto | undefined {
    return this.products.find(p => p.id === this.saleForm.productId);
  }

  calculateTotal(): number {
    return this.saleForm.quantitySold * this.saleForm.unitPrice;
  }
}
