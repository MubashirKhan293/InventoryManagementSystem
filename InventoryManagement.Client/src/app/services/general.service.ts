import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class GeneralService {

  constructor() { }

    async showConfirmationDialog(title: string, text: string, confirmButtonText: string) {
    const result = await Swal.fire({
      title: title,
      text: text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#1AC0A1',
      cancelButtonColor: '#d33',
      confirmButtonText: confirmButtonText
    });

    return result.isConfirmed;
  }

}
