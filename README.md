# QTKD — Hệ thống Quản trị Kinh doanh (Phase 2 · Tuần 05)

Project **ASP.NET Core Web API (.NET 10)** kết nối **PostgreSQL** bằng **Entity Framework Core**, xây dựng nền tảng cho hệ thống Quản trị Kinh doanh (Công ty ABC — dịch vụ logistics & khai thác cảng).

- **Git Repository:** *(Cập nhật link GitHub/GitLab cá nhân sau khi push)*
- **Công nghệ:** C# · .NET 10 · ASP.NET Core Web API · EF Core 10 · PostgreSQL · Swagger (Swashbuckle)
- **Phạm vi Tuần 05:** Khởi tạo solution phân tầng (`Controllers → Services → Repositories → QtkdDbContext → PostgreSQL`), tạo 4 entity (`Customer`, `Contract`, `PriceList`, `PriceItem`), migration `InitialCreate`, dữ liệu mẫu và API đọc dữ liệu.

---

## 1. Cấu trúc Repository

```text
.
├── QTKD.sln
├── README.md
├── .gitignore
├── docs/
│   ├── architecture.md       # Sơ đồ thành phần solution, ERD và đối chiếu Phase 1
│   └── verify-db.sql         # Câu lệnh SQL kiểm tra bảng, PK/FK và dữ liệu mẫu trong PostgreSQL
└── src/
    └── QTKD.Api/
        ├── Controllers/      # CustomersController, ContractsController, PriceListsController
        ├── DTOs/             # CustomerResponse, ContractResponse, PriceListResponse
        ├── Services/         # ICustomerService, IContractService, IPriceListService
        ├── Repositories/     # ICustomerRepository, IContractRepository, IPriceListRepository
        ├── Models/           # Customer, Contract, PriceList, PriceItem, QtkdStatuses
        ├── Data/             # QtkdDbContext, QtkdDbSeeder
        ├── Middleware/       # ApiExceptionMiddleware
        ├── Migrations/       # InitialCreate
        ├── QTKD.Api.http     # File gọi thử API nhanh trong VS Code
        └── Program.cs
```

Xem chi tiết kiến trúc phân tầng, sơ đồ ERD và các giả định đối chiếu Phase 1 tại [docs/architecture.md](docs/architecture.md).

---

## 2. Yêu cầu môi trường

- **.NET SDK 10** (`dotnet --version`)
- **PostgreSQL** (đang chạy trên cổng `5432`)
- **EF Core CLI tool** (nếu chưa cài):

```powershell
dotnet tool install --global dotnet-ef --version 10.0.3
```

---

## 3. Hướng dẫn cấu hình & chạy trên máy mới

### Bước 1: Tạo database trong PostgreSQL
Mở **pgAdmin** hoặc **psql** và tạo một database trống tên `qtkd_db`:

```sql
CREATE DATABASE qtkd_db;
```

### Bước 2: Cấu hình chuỗi kết nối PostgreSQL (không lưu mật khẩu lên Git)
File `src/QTKD.Api/appsettings.json` chỉ chứa mẫu `Host`, `Port`, `Database`, `Username` và **không chứa mật khẩu**.
Chạy lệnh sau tại thư mục gốc repository (thay `MAT_KHAU_CUA_BAN` bằng mật khẩu PostgreSQL trên máy của bạn):

```powershell
dotnet user-secrets set "ConnectionStrings:QtkdDb" "Host=localhost;Port=5432;Database=qtkd_db;Username=postgres;Password=MAT_KHAU_CUA_BAN" --project src/QTKD.Api
```

> Lưu ý: User Secrets được lưu riêng trong hồ sơ người dùng của hệ điều hành, không nằm trong mã nguồn nộp bài hoặc Git repository.

### Bước 3: Chạy EF Core Migration để tạo bảng
Tại thư mục gốc repository:

```powershell
dotnet ef database update --project src/QTKD.Api --startup-project src/QTKD.Api
```

Lệnh này áp dụng migration `InitialCreate`, tạo 4 bảng `Customers`, `Contracts`, `PriceLists`, `PriceItems` cùng đầy đủ khóa chính, khóa ngoại và unique index trong `qtkd_db`.

### Bước 4: Khởi động Web API và tự động nạp dữ liệu mẫu

```powershell
dotnet run --project src/QTKD.Api
```

Khi khởi động lần đầu (nếu bảng `Customers` đang trống), `QtkdDbSeeder` sẽ tự động thêm dữ liệu mẫu:
- **2 Customer:**
  1. `KH-MP01` — **Công ty Minh Phát** (`Active`)
  2. `KH-HD02` — **Công ty TNHH Vận tải Hải Đăng** (`Active`)
- **1 Contract ở trạng thái `Draft`:**
  - `HD-2026-018` thuộc **Công ty Minh Phát**, hiệu lực `2026-12-01` → `2027-11-30`, giá trị `450,000,000`.
- **1 PriceList kèm 2 PriceItem:**
  - *Bảng giá dịch vụ cảng & lưu kho 2026 - Minh Phát* gồm `BX-CONT20` (1,250,000 / Container) và `LK-KHO` (85,000 / Tấn/ngày).

---

## 4. Kiểm thử API & Đối chiếu PostgreSQL

Mở **Swagger UI** tại: `http://localhost:5095/swagger` (hoặc dùng file `src/QTKD.Api/QTKD.Api.http` / Postman).

### Danh sách endpoint Tuần 05

| Method | Endpoint | Ghi chú | Kết quả mong đợi |
| --- | --- | --- | --- |
| `GET` | `/api/customers` | **Bắt buộc TH5** — lấy danh sách khách hàng | `200 OK` (2 khách hàng mẫu) |
| `GET` | `/api/customers/{id}` | Lấy chi tiết một khách hàng | `200 OK` hoặc `404 Not Found` |
| `GET` | `/api/contracts/{id}` | **Bắt buộc TH5** — lấy chi tiết hợp đồng kèm tên khách hàng | `200 OK` với `id=1` (`HD-2026-018`, `Draft`), `404` nếu không tồn tại |
| `GET` | `/api/contracts` | Lấy danh sách hợp đồng | `200 OK` |
| `GET` | `/api/pricelists` | Lấy danh sách bảng giá kèm `PriceItems` | `200 OK` (1 bảng giá, 2 hạng mục) |
| `GET` | `/api/pricelists/{id}` | Lấy chi tiết bảng giá theo Id | `200 OK` hoặc `404 Not Found` |

### Đối chiếu với PostgreSQL
Chạy các câu lệnh trong [docs/verify-db.sql](docs/verify-db.sql) bằng **pgAdmin** (Query Tool) hoặc **psql** trên database `qtkd_db` để xác nhận:
1. Đủ 4 bảng nghiệp vụ + bảng `__EFMigrationsHistory`.
2. Đủ khóa chính (PK) và khóa ngoại (`Contracts.CustomerId → Customers.Id`, `PriceItems.PriceListId → PriceLists.Id`, `PriceLists.CustomerId → Customers.Id`).
3. Dữ liệu trả về từ `GET /api/customers` và `GET /api/contracts/1` khớp 100% với các dòng trong bảng PostgreSQL.
