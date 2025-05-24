export interface PurchaseDto {
  id: number;
  productId: number;
  productName: string;
  quantityPurchased: number;
  unitPrice: number;
  totalAmount: number;
  purchaseDate: Date;
  supplierName: string;
}

export interface CreatePurchaseDto {
  productId: number;
  quantityPurchased: number;
  unitPrice: number;
  supplierName: string;
}