import { CommonModule } from '@angular/common';
import { Component, Input, SimpleChanges } from '@angular/core';
declare var bootstrap: any;

@Component({
  selector: 'app-modal-errors',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal-errors.component.html',
  styleUrl: './modal-errors.component.scss'
})
export class ModalErrorsComponent {
  @Input() errores: string[] = [];
  erroresMultiples: string[] = [];

  ngOnChanges(changes: SimpleChanges) {
    if (changes['errores']) {
      this.erroresMultiples = [...this.errores];
    }
  }

  open() {
    const modalEl = document.getElementById('erroresModal');
    const modal = new bootstrap.Modal(modalEl, {
      backdrop: false,    
      keyboard: true  
    });
    modal.show();
  }
}
