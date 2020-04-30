import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpHandler } from '@angular/common/http';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',


})

export class HomeComponent {
  public stories: HackerNewsInterface[];
  public story: HackerNewsInterface;
  public searchText: any;
  public response: APIResponse;
  private count: number;
  public errorMessage: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, @Inject('HACKER_URL') hackerUrl: string)
  {
    this.stories = [];
    this.count = 0;
    http.get<APIResponse>(baseUrl + "/GetCacheValues").subscribe(result => {
      this.response = result;
      if (this.response.isSuccessful) {
        for (let i of this.response.data) {
          try
          {
            this.count = this.count + 1;
            this.story = i;
            let jsdate = new Date(this.story.time * 1000)
            this.story.friendlytime = jsdate.toLocaleDateString() + " : " + jsdate.toLocaleTimeString();
            this.stories.push(this.story);
          }
          catch(ex) {
            console.log(ex);
          }
        }
      }
      else {
        http.get<APIResponse>(baseUrl).subscribe(result => {
          this.response = result;
          if (this.response.isSuccessful) {
            for (let id of this.response.data) {
              http.get<HackerNewsInterface>(hackerUrl + 'item/' + id + ".json").subscribe(result => {
                this.count = this.count + 1;
                this.story = result;
                let jsdate = new Date(this.story.time * 1000)
                this.story.friendlytime = jsdate.toLocaleDateString() + " : " + jsdate.toLocaleTimeString();
                this.stories.push(this.story);
                if (this.count == 500) {
                  this.cacheData(baseUrl);
                 /*var data = JSON.stringify(this.stories);
                  const headerDict = {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                    'Access-Control-Allow-Headers': 'Content-Type',
                  }
                  const requestOptions =
                  {
                    headers: new HttpHeaders(headerDict),
                  };
                  http.post<APIResponse>(baseUrl + "/CacheValues", data, requestOptions).subscribe(result => {
                    this.response = result;
                    if (this.response.isSuccessful) {
                      console.error(this.response.friendlyMessage);
                    }
                    else {
                      console.error(this.response.errorMessage);
                    }
                  });*/
                }

              });
            }
          }
          else {
            this.errorMessage = this.response.friendlyMessage;
          }
        }, error => console.error(error))
      }
    });
  }

  cacheData(@Inject('BASE_URL') baseUrl: string)
  {
    var data = JSON.stringify(this.stories);
    const headerDict = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Access-Control-Allow-Headers': 'Content-Type',
    }
    const requestOptions =
    {
      headers: new HttpHeaders(headerDict),
    };
    this.http.post<APIResponse>(baseUrl + "/CacheValues", data, requestOptions).subscribe(result => {
      this.response = result;
      if (this.response.isSuccessful) {
        console.error(this.response.friendlyMessage);
      }
      else {
        console.error(this.response.errorMessage);
      }
    });
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
