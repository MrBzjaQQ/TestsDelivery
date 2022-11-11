import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UrlService {

  constructor() { }

  makeUrlWithId(path: string, id: number): string {
    return `${path}${id}`;
  }
  
}
