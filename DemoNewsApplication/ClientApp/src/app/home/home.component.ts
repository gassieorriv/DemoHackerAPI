import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent
{
  public storyIds: NewsStories[];
  public stories: HackerNewsInterface[];
  public story: HackerNewsInterface;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    http.get<NewsStories[]>(baseUrl + 'newstories.json').subscribe(result => {
      this.storyIds = result;
      this.stories = [];
      for (let id of this.storyIds)
      {
        http.get<HackerNewsInterface>(baseUrl + 'item/' + id + ".json").subscribe(result => {
        this.story = result;
        let jsdate = new Date(this.story.time * 1000)
        this.story.friendlytime = jsdate.toLocaleDateString() + " : " + jsdate.toLocaleTimeString();
        this.stories.push(this.story);
       })
      }
    }, error => console.error(error));
  }
}

interface HackerNewsInterface {
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
}

interface NewsStories {
  ids: number;
}
