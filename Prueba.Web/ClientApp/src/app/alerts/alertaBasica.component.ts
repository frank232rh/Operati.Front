import { Component, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
@Component({
  selector: 'alertaBasica',
  templateUrl: './alertaBasica.component.html',
  styles: [],
  standalone: true,
  imports: [MatButtonModule, MatDialogModule]
})
export class alertaBasicaComponent {
  constructor(
    public dialogRef: MatDialogRef<alertaBasicaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  close() {
    this.dialogRef.close();
  }
}
