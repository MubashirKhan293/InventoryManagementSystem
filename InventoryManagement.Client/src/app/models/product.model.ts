export interface ProductDto {
  id: number;
  name: string;
  description: string;
  quantity: number;
  createdDate: Date;
}

export interface ProductInputDto {
  name: string;
  description: string;
  quantity: number;
}