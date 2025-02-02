import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { User } from '../../../../core/domain/models/user.model';
import { Roles } from '../../../../core/enums/roles.enum';
import { MatDialog } from '@angular/material/dialog';
import { UserFormComponent } from '../user-form/user-form.component';
import { UsersRepository } from '../../../../data/repositories/users.repository';
import { debounceTime, distinctUntilChanged, finalize, Observable, Subject } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['id', 'username', 'role', 'actions'];
  dataSource: MatTableDataSource<User> = new MatTableDataSource();
  totalUsers: number = 0;
  pageSize: number = 5;
  currentPage: number = 1;
  users$  = new Observable<User[]>();
  searchQuery: string = '';
  loading = false
  searchSubject = new Subject<string>(); // 🔹 Add a Subject for debouncing
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private dialog: MatDialog, private usersRepository: UsersRepository) {}

  ngOnInit(): void {
    this.getUsers();
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;

  // ✅ Debounce search input
  this.searchSubject.pipe(
    debounceTime(500),  // 🔹 Wait 500ms after last keystroke
    distinctUntilChanged() // 🔹 Only trigger if value is different from the last one
  ).subscribe(value => {
    this.searchQuery = value;
    this.getUsers();
  });
  }

    // Get sorting parameters from MatSort
    getSortingParams() {
      const sortBy = this.sort?.active || 'id'; // Default to 'id' column
      const ascending = this.sort?.direction === 'desc'; // True if ascending, false if descending
      return { sortBy, ascending };
    }
  getUsers(): void {
    this.loading = true;
    const { sortBy, ascending } = this.getSortingParams(); // Get sorting parameters
    this.usersRepository
      .getUsersPaginated( this.currentPage,this.pageSize,this.searchQuery, sortBy, ascending)    
      .pipe(
      finalize(()=>this.loading = false) )
      .subscribe(
        (response) => {
          this.dataSource.data = response.items;
          this.totalUsers = response.totalCount; // Total count of users for pagination
        },
        (error) => {
          console.error('Error fetching users:', error);
        }
      );
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim();
    this.searchSubject.next(filterValue); // 🔹 Send the input to debounce
  }
  pageChanged(event: any): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.getUsers();
  }

  openAddUserDialog(): void {
    const dialogRef = this.dialog.open(UserFormComponent, {
      width: '400px',
      data: {}, // Empty data for add mode
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.usersRepository.addUser(result).subscribe(() => this.getUsers());
      }
    });
  }

  openEditUserDialog(user: User): void {
    const dialogRef = this.dialog.open(UserFormComponent, {
      width: '400px',
      data: user,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.usersRepository.editUser(user.id, result).subscribe(() => this.getUsers());
      }
    });
  }

  deleteUser(id: number): void {
    this.usersRepository.deleteUser(id).subscribe(() => this.getUsers());
  }

  get userRoles() {
    return Roles;
  }
}
