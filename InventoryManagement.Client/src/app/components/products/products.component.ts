import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ProductDto, ProductInputDto } from '../../models/product.model';
import { ProductService } from '../../services/product.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ColumnMode, DatatableComponent, NgxDatatableModule, SelectionType, SortType } from '@swimlane/ngx-datatable';
import { CommonModule } from '@angular/common';
import { GeneralService } from '../../services/general.service';

@Component({
  selector: 'app-products',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgxDatatableModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent {
  products: ProductDto[] = [];
  filteredProducts: ProductDto[] = [];
  searchTerm: string = '';
  showAddForm: boolean = false;
  editingProduct: ProductDto | null = null;
  productNameExists: boolean = false;
  
  productForm: ProductInputDto = {
    name: '',
    description: '',
    quantity: 0
  };

  // Pagination
  currentPage = 0;
  pageSize = 10;
  totalElements = 0;

  constructor(
    private _productService: ProductService,
    private _generalService: GeneralService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this._productService.getAllProducts().subscribe({
      next: (data) => {
        this.products = data;
        this.filteredProducts = data;
        this.totalElements = data.length;
      },
      error: (error) => console.error('Error loading products:', error)
    });
  }

  onSearch(): void {
    if (!this.searchTerm) {
      this.filteredProducts = this.products;
    } else {
      this.filteredProducts = this.products.filter(product =>
        product.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        product.description.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }
    this.totalElements = this.filteredProducts.length;
    this.currentPage = 0;
  }

  onAddProduct(): void {
    this.showAddForm = true;
    this.editingProduct = null;
    this.resetForm();
  }

  onEditProduct(product: ProductDto): void {
    this.showAddForm = true;
    this.editingProduct = product;
    this.productForm = {
      name: product.name,
      description: product.description,
      quantity: product.quantity
    };
  }

  onSubmit(): void {
    if (this.editingProduct) {
      this._productService.updateProduct(this.editingProduct.id, this.productForm).subscribe({
        next: () => {
          this.loadProducts();
          this.cancelForm();
        },
        error: (error) => console.error('Error updating product:', error)
      });
    } else {
      this._productService.createProduct(this.productForm).subscribe({
        next: () => {
          this.loadProducts();
          this.cancelForm();
        },
        error: (error) => console.error('Error creating product:', error)
      });
    }
  }

  async onDeleteProduct(productId: number, productName: string) {
    const confirmed = await this._generalService.showConfirmationDialog(
      'Confirm Deletion',
      `Are you sure you want to delete "${productName}"? This will also delete all related sales and purchases.`,
      'Yes, delete it!'
    );

    if (confirmed) {
      this.deleteProduct(productId);
    }
  }

  deleteProduct(id: number): void {
    this._productService.deleteProduct(id).subscribe({
      next: () => this.loadProducts(),
      error: (error) => console.error('Error deleting product:', error)
    });
  }

  cancelForm(): void {
    this.showAddForm = false;
    this.editingProduct = null;
    this.resetForm();
  }

  resetForm(): void {
    this.productForm = {
      name: '',
      description: '',
      quantity: 0
    };
  }

 checkProductNameExists(productName: string) {
  if (!productName || productName.trim() === '') {
    this.productNameExists = false;
    return;
  }

  this._productService.checkProductNameExists(productName).subscribe({
    next: (response: boolean) => {
      this.productNameExists = response;
      console.log("checking product name exists or not", response);
    },
    error: (error: any) => {
      console.error("error validating product name", error);
      this.productNameExists = false; // assume not exists on error
    }
  });
}

  onPageChange(event: any): void {
    this.currentPage = event.offset;
  }

}
