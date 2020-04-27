import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  
  
})

export class HomeComponent
{
    public stories: HackerNewsInterface[];
    public story: HackerNewsInterface;
    public searchText: any;
    public response: APIResponse;
  public errorMessage: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, @Inject('HACKER_URL') hackerUrl: string)
    {
      http.get<APIResponse>(baseUrl).subscribe(result => {
        this.response = result;
        if (this.response.isSuccessful) {
          this.stories = [];
          for (let id of this.response.data) {
            http.get<HackerNewsInterface>(hackerUrl + 'item/' + id + ".json").subscribe(result => {
              this.story = result;
              let jsdate = new Date(this.story.time * 1000)
              this.story.friendlytime = jsdate.toLocaleDateString() + " : " + jsdate.toLocaleTimeString();
              this.stories.push(this.story);
            })
          }
        }
        else {
          this.errorMessage = this.response.friendlyMessage;
        }
      }, error => console.error(error));
    }
 }

export interface HackerNewsInterface {
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

export interface APIResponse {
  data: any;
  errorMessage: string;
  isSuccessful: boolean;
  friendlyMessage: string;
}
