import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { 
    path: 'products', 
    loadComponent: () => import('./components/products/products.component').then(m => m.ProductsComponent) 
  },
  { 
    path: 'sales', 
    loadComponent: () => import('./components/sales/sales.component').then(m => m.SalesComponent) 
  },
  { 
    path: 'purchases', 
    loadComponent: () => import('./components/purchases/purchases.component').then(m => m.PurchasesComponent) 
  }
];