import { Pipe, PipeTransform } from '@angular/core';
import { HomeComponent, HackerNewsInterface } from '../home/home.component';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform, HackerNewsInterface
{
    id: string;
    deleted: boolean;
    type: string;
    by: string;
    time: number;
    friendlytime: string;
    text: string;
    dead: boolean;
    parent: number;
    poll: string;
    kids: number[];
    url: string;
    score: string;
    title: string;
    descendants: number;

  constructor(component: HomeComponent) { }
  transform(items:HackerNewsInterface[], searchText: string): any[] {
    if (!items) return [];
    if (!searchText) return items;
    searchText = searchText.toLowerCase();
    return items.filter(it => {
      return it.title.toLowerCase().includes(searchText.toLowerCase());
    });
  }
}
