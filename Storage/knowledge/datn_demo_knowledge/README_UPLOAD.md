# DATN Demo Data for Gemini File Search Store

Bộ dữ liệu demo này để Gemini trả lời theo dữ liệu truy xuất nguồn gốc (sản phẩm, lô, mã QR, chuỗi cung ứng).

## Quy mô dataset (default)
- Sản phẩm: 50
- Mỗi sản phẩm: 3 lô
- Mỗi lô: 30 mã QR
- Tổng mã QR: 4500

## File trong thư mục
- enterprises.jsonl: Doanh nghiệp
- products.jsonl: Sản phẩm
- batches.jsonl: Lô hàng
- tracecodes.jsonl: Mã QR (mã truy xuất)
- supply_steps.jsonl: Các bước chuỗi cung ứng theo lô
- standards.jsonl: Tiêu chuẩn
- trace_lookup.jsonl: Record “tóm tắt tra cứu” để tăng độ chính xác khi hỏi theo mã QR

## Upload lên Gemini File Search Store (Windows CMD hoặc powersell - khuyến nghị)
Store của:
- fileSearchStores/exoticknowledge-pkm8hjba73eb

upload từng file bằng curl (Content-Type dùng text/plain cho jsonl là ổn):

Ví dụ upload products.jsonl:
curl -X POST "https://generativelanguage.googleapis.com/upload/v1beta/fileSearchStores/exoticknowledge-pkm8hjba73eb:uploadToFileSearchStore" ^
  -H "x-goog-api-key: YOUR_KEY" 
  -H "X-Goog-Upload-Protocol: raw" 
  -H "Content-Type: text/plain" 
  --data-binary "@/mnt/data/datn_demo_knowledge/products.jsonl"

Làm tương tự cho các file còn lại.

## Câu hỏi test
1) "Mã QR-SP0001-01-0001 thuộc sản phẩm nào, lô nào?"
2) "Lô LHSP0001-01 đi qua những bước chuỗi cung ứng nào?"
3) "Sản phẩm SP0001 thuộc doanh nghiệp nào? nguồn gốc ở đâu?"
4) "Cho tôi tóm tắt thông tin truy xuất cho mã QR-SP0002-02-0010"
