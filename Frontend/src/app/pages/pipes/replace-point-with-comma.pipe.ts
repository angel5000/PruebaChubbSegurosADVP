import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'replacePointWithComma',
  standalone: true
})
export class ReplacePointWithCommaPipe implements PipeTransform {

  transform(value: any): string {
    if (value == null) return '';
    return value.toString().replace('.', ',');
  }

}
