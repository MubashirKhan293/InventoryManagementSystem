export interface SaleDto {
  id: number;
  productId: number;
  productName: string;
  quantitySold: number;
  unitPrice: number;
  totalAmount: number;
  saleDate: Date; 
  customerName: string;
}

export interface CreateSaleDto {
  productId: number;
  quantitySold: number;
  unitPrice: number;
  customerName: string;
}