import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_modules/user';
import { PaginatedResult } from '../_modules/pagination';
import { map } from 'rxjs/operators';
import { UserParams } from '../_modules/userParams';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) {}
  baseUrl = environment.apiUrl;

  getUsers(page?, itemsPerPage?, userParams?: UserParams): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult();

    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('minAge', userParams.minAge.toString());
      params = params.append('maxAge', userParams.maxAge.toString());
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    return this.http
      .get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
        .pipe(
          map((response) => {
            paginatedResult.result = response.body;
            if (response.headers.get('Pagination') != null) {
              paginatedResult.pagination = JSON.parse(
                response.headers.get('Pagination')
              );
              return paginatedResult;
            }
          })
      );
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(userId: number, photoId: number) {
    return this.http.post(
      this.baseUrl + 'users/' + userId + '/photos/' + photoId + '/setMain',
      {}
    );
  }

  deletePhoto(userId: number, photoId: number) {
    return this.http.delete(
      this.baseUrl + 'users/' + userId + '/photos/' + photoId
    );
  }
}
