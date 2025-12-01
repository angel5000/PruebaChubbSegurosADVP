import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'replacePointWithComma',
  standalone: true
})
export class ReplacePointWithCommaPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }

}
