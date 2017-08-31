from http.server import BaseHTTPRequestHandler, HTTPServer

class MyHandler(BaseHTTPRequestHandler):

    def __init__(self, request, client_address, server):
        super().__init__(request, client_address, server)

    def do_GET(self):
        print("request:")
        print(self.path)
        if self.path.startswith("/function/?id="):
            #TODO: extract msg
            msg = self.path[14:28] # "WP000000000000"
            print(msg)
            self.send_response(200)
            self.send_header('Content-type', 'text')
            self.end_headers()
            self.wfile.write(b"OK")
            self.robot.React(msg)
            #self.server.handle()

        else:
            self.send_error(404)
            #self.server.handle()