import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldDefaultOptions, MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator, MatPaginatorIntl, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { startWith, switchMap } from 'rxjs';
import { BaseResponse } from '../../../models/BaseApiResponse';
import { TableColumns, TableFooter } from '../../../models/list-table-interface';
import { getEsPaginatorIntl } from '../../../paginator-intl/es-paginator-intl';
import { DefaultService } from '../../../services/default.service';
import { SharedModule } from '../../../shared.module';

@Component({
  selector: 'app-list-table',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatSortModule, MatTooltipModule, MatIconModule,
    MatPaginatorModule, FormsModule, SharedModule],
  templateUrl: './list-table.component.html',
  styleUrl: './list-table.component.scss',
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: getEsPaginatorIntl()
    }, {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: { appereance: "standard" } as MatFormFieldDefaultOptions,
    }
  ]
})
export class ListTableComponent<T> implements OnInit, AfterViewInit, OnChanges {
  @Input() service?: DefaultService;
  @Input() colums?: TableColumns<T>[];
  @Input() getInputs?: any;
  @Input() Numrecords?: number = 5;
  @Input() sortBy?: string;
  @Input() sortDir: string = "asc";
  @Input() footer: TableFooter<T>[] = [];
  @Input() manualData?: any[] | null = null;
  @Input() Pantalla?: string;
  @Input() filtros: any = {};
  @Output() totalFallidos: EventEmitter<number> = new EventEmitter<number>();
  @Output() rowClick = new EventEmitter<{ action: string; row: T }>();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  changesGetInputs = new EventEmitter<T>();
  dataSource = new MatTableDataSource<T>();
  visibleColumns: Array<keyof T | string>;
  visibleFooter: Array<keyof T | string | object>;
  paginatorOptions = {
    pageSizeOptions: [5, 10, 20, 50],
    pageSize: 5,
    pageLenght: 0,
  };

  ngOnInit(): void {

    if (this.manualData) {
      this.changesGetInputs.emit();
      this.dataSource = new MatTableDataSource(this.manualData);
      this.paginatorOptions.pageLenght = this.manualData.length;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.changesGetInputs.emit();
      return;
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnChanges(changes: SimpleChanges): void {

    if (changes['colums']) {
      this.setVisibleColumns();
    }

    if (changes['getInputs'] && this.paginator) {
      this.paginator.pageIndex = 0;
      this.changesGetInputs.emit();
    }

    if (changes['filters']) {
      this.changesGetInputs.emit();
    }

    if (changes['manualData'] && this.manualData) {
      this.dataSource.data = [...this.manualData]; // forzar nueva referencia
      if (this.paginator) {
        this.dataSource.paginator = this.paginator;
      }
    }
  }

  setVisibleColumns() {
    this.visibleColumns = this.colums
      .filter((columns: any) => columns.visible)
      .map((columns: any) => columns.property)
  }

  ngAfterViewInit(): void {
    if (!this.manualData && this.manualData !== undefined) {
      this.getDataByService();
    }
    this.sortChanges();
    this.paginatorChanges()

  }

  paginatorChanges() {
    this.paginator.page.subscribe(() => {
      this.changesGetInputs.emit();
    });
  }

  sortChanges() {
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
      this.changesGetInputs.emit();
    });
  }
  async getDataByService() {
    this.changesGetInputs
      .pipe(startWith("")
        , switchMap(() => {
          return this.service.Obtenerdatos()
        })

      ).subscribe((data: BaseResponse) => {
        console.log(data);

        if (data.isSucces) {
          this.setData(data);
          console.log("Datos recibidos: ", data.data);
        } else {
          console.warn("Error al cargar los datos");
        }
      });
  }

  setData(data: BaseResponse) {
    if (!data.isSucces) return;
    let processedData: any[] = [];

    if (this.Pantalla === 'asegurados' && !this.filtros.cedula) {
      const grouped = Object.values(
        data.data.reduce((acc, item: any) => {
          if (!acc[item.cedula]) {
            acc[item.cedula] = {
              ...item,
              seguros: []
            };
          }
          acc[item.cedula].seguros.push({
            idusrseguros: item.idusrseguros,
            nmbrseguro: item.nmbrseguro,
            codseguro: item.codseguro,
            sumasegurada: item.sumasegurada,
            prima: item.prima,
            fechacontrataseguro: item.fechacontrataseguro,
            estado:item.estado
          });
          return acc;
        }, {}));

      processedData = grouped;
      this.paginatorOptions.pageLenght = grouped.length;
    } 
    else {
      processedData = data.data;
      this.paginatorOptions.pageLenght = data.data.length;
    }

   if (this.filtros.numFilter) {
      const searchValue = this.filtros.textFilter;
      processedData = processedData.filter((item: any) => {
        if (searchValue) {
          const value = typeof searchValue === 'string'
          ? searchValue
          : Object.values(searchValue)[0]; // toma el primer valor del objeto

        return (
          item.idseguro?.toString().includes(value) ||
          item.edadmin?.toString().includes(value) ||
          item.edadmax?.toString().includes(value) ||
          item.cedula?.toString().includes(value) ||
          item.nmbrcompleto?.toLocaleLowerCase().includes(value) ||
          item.codseguro?.toString().toLocaleLowerCase().includes(value)||
          item.nmbrseguro?.toString().toLocaleLowerCase().includes(value)
        );
        }
        return Object.keys(this.filtros).every(key => {

          if (!item[key]) return false;
          return item[key].toString().toLowerCase().includes(this.filtros[key].toLowerCase());
        });
      });
    }

    this.dataSource.data = processedData;
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.setVisibleColumns();
  }

}
