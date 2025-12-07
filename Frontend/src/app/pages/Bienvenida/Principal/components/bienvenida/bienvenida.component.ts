import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-bienvenida',
  templateUrl: './bienvenida.component.html',
  styleUrl: './bienvenida.component.scss',

})

export class BienvenidaComponent implements OnInit {


  form: FormGroup
  paymentId: string | null = null;
  payerId: string | null = null;
  token: string | null = null;
  pagado: boolean = false;
  filtro: boolean;
  precio: any;
  idplan: any;
  constructor(private router: Router) {


  }
  ngOnInit(): void {

  }

  Polizas() {
    this.router.navigate(["/seguros"]);
  }
  Asegurados() {
    this.router.navigate(["/asegurados"]);
  }


}



