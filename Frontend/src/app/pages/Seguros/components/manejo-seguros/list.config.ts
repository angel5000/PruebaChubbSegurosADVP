import { SearchOptions } from "../../../../shared/models/SearchOptions.interface";
import { GenericValidators } from "../../../../shared/validators/generic-validators";

const searchOptions: SearchOptions[] =[
    {
        label:"Codigo del Seguro",
        value:1,
        placeholder: "Buscar por codigo del seguro",
        validation:[GenericValidators.alphanumeric],
        validation_desc:"",
        min_lenght:1,
    },
]

  function getTableColumns() {
    return [
        {
            label: "ID",
            csslabel: ["font-bold", "text-sm"],
            property: "idseguro",
            cssProperty: ["font-semibold", "text-sm", "text-left"],
            type: "number",
            sticky: true,
            sort: true,
            sortProperty: "idseguro", 
            visible: true,
            download: true,
          },
      {
        label: "Nombre Seguro",
        csslabel: ["font-bold", "text-sm"],
        property: "nmbrseguro",
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        type: "text",
        sticky: true,
        sort: true,
        sortProperty: "nmbrseguro", 
        visible: true,
        download: true,
      },
      {
        label: "Codigo Seguro",
        csslabel: ["font-bold", "text-sm"],
        property: "codseguro",
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        type: "text",
        sticky: true,
        sort: true,
        sortProperty: "codseguro", 
        visible: true,
        download: true,
      },
      {
        label: "Suma Seguro",
        property: "sumasegurada",
        type: "currency",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
      {
        label: "Prima",
        property: "prima",
        type: "currency",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
      {
        label: "Edad Admitida",
        property: "rangoEdad",
        type: "text",
        csslabel: ["font-bold", "text-sm"],
        cssProperty: ["font-semibold", "text-sm", "text-left"],
        sticky: false,
        sort: true,
        visible: true,
        download: true,
      },
      {
        label:"",
        csslabel:[],
        cssProperty:["bi bi-pen fs-5"],
        cssSubProperty:['bi bi-pen fs-5'],
        property: "icEdit",
        type:"icon",
        tooltip:"editar",
    action:"edit",
    sticky:false,
    sort:false,
    visible:true,
    download:false
    
    },
    {
      label: "",
      csslabel: [],
      cssProperty: [''],
      property: "eye",
      cssSubProperty: ["bi bi-eye fs-5"],
      type: "icon",
      action: "ver",
      visible: true,
      tooltip: "ver Asegurados",
      sticky: false,
      sort: false,
      download: false
    },
    {
        label:"",
        csslabel:[],
        cssProperty:[''],
        property: "icDelete",
        cssSubProperty:["bi bi-trash3 fs-5"],
        type:"icon",
    action:"remove",
    tooltip:"eliminar",
    sticky:false,
    sort:false,
    visible:true,
    download:false
    
    }
    ];
  }

const filters={
    numFilter:"",
    textFilter:"",
    stateFilter: null,
    startDate: null,
    endDate: null,
    refresh:false
}

const getInputs: string="";

export const ComponentSettings={
    tableColumns:getTableColumns(),
    getInputs,
    filters:filters,
    filters_dates_active:false,
    searchOptions: searchOptions,
   
    }
  